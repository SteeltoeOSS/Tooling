#!/usr/bin/env bash

basedir=$(dirname $0)/..
featureTestDir=$basedir/feature
errors=0

for project in $featureTestDir/*.Feature
do
    dotnet test $project
    errors=$(($errors + $?))
done

if [[ $errors > 0 ]]
then
	echo "$errors unit test(s) failed" >&2
    exit $errors
fi
