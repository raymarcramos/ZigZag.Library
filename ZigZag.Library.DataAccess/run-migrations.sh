#!/bin/bash

if ! command -v dotnet &> /dev/null
then
    echo ".NET SDK is not installed. Please install it to continue."
    exit 1
fi

if [ -z "$1" ]; then
    read -p "Please enter a migration name: " migrationName
else
    migrationName=$1
fi

echo "Adding new migration '$migrationName'..."
dotnet ef migrations add "$migrationName"

echo "Applying migration to the database..."
dotnet ef database update

echo "Migration '$migrationName' completed."
