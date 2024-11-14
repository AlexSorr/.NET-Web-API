#!/bin/bash

PROJECT_ROOT="."

# Массив исключений - пути к папкам, которые нужно игнорировать
EXCLUDE_DIRS=("node_modules" "bin" "obj" "Logs", "Migrations", "Properties")

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