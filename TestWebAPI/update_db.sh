#!/bin/bash

migration_path=$PWD/Migrations

if [ ! -d "$migration_path" ]; then 
    echo -n "Не найден каталог с миграциями"
    exit 1
fi

# Добавить миграцию, обновить БД
dotnet ef migrations add "migration-$(date +'%Y-%m-%d_%H-%M-%S')"
dotnet ef database update