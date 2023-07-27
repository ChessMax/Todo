#!/usr/bin/env bash

name="$1"

if [ "${name}" == "" ]; then
    echo "Invalid migration name"
    exit 1
fi

dotnet ef migrations add "$name"