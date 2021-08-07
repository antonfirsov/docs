#!/bin/bash
called_path=${0%/*}
project_file=$called_path/tools/blog-new-post/blog-new-post.csproj

dotnet run --project $project_file -- $*