#!/usr/bin/env bash

basedir=$(dirname $0)/..
unitTestDir=$basedir/test

errors=0

for project in $unitTestDir/*.Test
do
    dotnet test $project
    errors=$(($errors + $?))
done

if [[ $errors > 0 ]]
then
	echo "$errors unit test(s) failed" >&2
    exit $errors
fi
