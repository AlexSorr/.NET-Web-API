#!/bin/bash
migrations_path="$PWD"/Migrations

echo -e "Migrations folder: $migrations_path"

if [ ! -d "$migrations_path" ]; then
    echo "Не найден каталог с миграциями по пути $migrations_path"
    exit 1
fi

rm -rvf "$migrations_path"/*