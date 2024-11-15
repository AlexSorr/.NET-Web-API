#!/bin/bash
# Хелпер для разработки на dotnet

# Запаковать старые логи
function archive_logs() {
    echo "Start logging archivation..."

    today=$(date +"%Y%m%d")
    archive_name="logs.zip"

    # Путь к каталогу с логами
    log_dir="$PWD/Logs"

    if [ ! -d "$log_dir" ]; then
        echo "No log directiory fount: $log_dir"
        exit 1
    fi

    # Перемещаемся в каталог с логами
    cd "$log_dir" || exit 1

    # Добавляем в архив все файлы, не относящиеся к сегодняшней дате
    for file in api_log*.txt; do
        # Проверяем, если файл не содержит сегодняшнюю дату - добавляем в архив
        if [[ $file != *"$today"* ]]; then
            zip -u "$archive_name" "$file"
            rm "$file"
        fi
    done

    echo "Done"
}

# Удалить старые миграции
function drop_migrations() {
    echo "Starting removing migrations..."

    migrations_path="$PWD"/Migrations

    # Проверка наличия папки Migrations
    if [ ! -d "$migrations_path" ]; then
        echo "No migrations folder found"
        exit 0
    fi
        
    # Подсчёт количества видимых элементов в папке
    count=$(find ./Migrations -maxdepth 1 -type f | grep -v "/\." | wc -l)

    echo -e "Path: $migrations_path\nElements count:$count"

    if [ $count -le 0 ]; then
        echo "No migrations found"
        exit 0
    fi

    read -p "Continue (y/n): " choice

    if [[ "$choice" != "y" && "$choice" != "Y" ]]; then
        echo "Abort"
        exit 0
    fi

    echo "Deleting migrations..."

    rm -rvf "$migrations_path"/*

    echo "Done"
}

# Создать новые миграции и обновить БД
function update_database() {
    echo "Database updating started..."

    migration_path=$PWD/Migrations
    if [ ! -d "$migration_path" ]; then 
        echo -n "Migrations directory not found"
        exit 1
    fi

    migration_name="migration-$(date +'%Y-%m-%d_%H-%M-%S')"
    echo -n "Creating migration $migration_name" 

    # Добавить миграцию, обновить БД
    dotnet ef migrations add "$migration_name" > /dev/null
    if [ $? -ne 0 ]; then
        echo "Creating migration error, abort"
        exit 1
    fi

    dotnet ef database update
}

# вывести структуру проекта
function proj_struct {
    PROJECT_ROOT="."

    # Массив исключений - пути к папкам, которые нужно игнорировать
    EXCLUDE_DIRS=("node_modules" "bin" "obj" "Logs" "Migrations" "Properties")

    # Функция для проверки, является ли папка исключением
    function is_excluded() {
        local dir="$1"
        for excluded in "${EXCLUDE_DIRS[@]}"; do
            if [[ "$dir" == *"/$excluded" ]]; then
                return 0 
            fi
        done
        return 1
    }

    function build_tree() {
        local dir="$1"
        local indent="$2"

        for item in "$dir"/*; do
            if [ -d "$item" ]; then
                if is_excluded "$item"; then
                    continue
                fi
                # Если это папка, выводим её с отступом
                echo "${indent}$(basename "$item")/"
                # Рекурсивно строим структуру для вложенных папок
                build_tree "$item" "$indent    "
            elif [ -f "$item" ]; then
                # Если это файл, выводим его с отступом
                echo "${indent}$(basename "$item")"
            fi
        done
    }

    build_tree "$PROJECT_ROOT" ""
}

# удалить бд
function drop_db {
    dotnet ef database drop
}

if [[ -z "$1" ]]; then
    echo "No commands found $1"
    exit 1
fi

# Выбор команды в зависимости от аргумента
case "$1" in
    --archive-logs)
        archive_logs
        ;;
    --drop-migrations)
        drop_migrations
        ;;
    --update-db)
        update_database
        ;;
    --drop-db)
        drop_db
        ;;
    --proj-struct)
        proj_struct
        ;;
    *)
        echo "No commands found $1"
        exit 1
        ;;
esac
