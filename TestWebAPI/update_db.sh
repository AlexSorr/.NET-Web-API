#!/bin/bash

migration_path=$PWD/Migrations

if [ ! -d "$migration_path" ]; then 
    echo -n "Migrations directory not found"
    exit 1
fi

migration_name="migration-$(date +'%Y-%m-%d_%H-%M-%S')"
echo -e "Creating migration: $migration_name" 

# Добавить миграцию, обновить БД
dotnet ef migrations add "$migration_name" >&2

if [ $? -ne 0 ]; then
    echo "Creating migration error, abort"
    exit 1
fi

dotnet ef database update