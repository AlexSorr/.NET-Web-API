#!/bin/bash
migrations_path="$PWD"/Migrations
migration_path=$PWD/Migrations

# Проверка наличия папки Migrations
if [ -d "$migration_path" ]; then
    # Подсчёт количества видимых элементов в папке
    count=$(find ./Migrations -maxdepth 1 -type f | grep -v "/\." | wc -l)
    echo -e "$migration_path \nElements count: $count"
else
    echo "No migrations folder found"
    exit 1
fi

read -p "Continue (y/n): " choice

if [[ "$choice" != "y" && "$choice" != "Y" ]]; then
    echo "Abort"
    exit 0
fi

echo "Deleting migrations"

rm -rvf "$migrations_path"/*

echo "Done"