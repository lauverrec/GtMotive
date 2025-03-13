#!/bin/bash
set -e

for project in $(find test -type f -name "*.csproj"); do
  echo "Execute tests into: $project"
  dotnet test "$project" --logger "console;verbosity=detailed"
done