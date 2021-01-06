#!/bin/bash
called_path=${0%/*}
project_file=$called_path/tools/blog-validate/blog-validate.csproj
categories_file=$called_path/categories.txt

dotnet run --project $project_file -- $called_path --categories $categories_file $*