#!/usr/bin/env bash

basedir=$(dirname $0)/..
unitTestDir=$basedir/test
rc=0

for project in $unitTestDir/*
do
    dotnet test $project
    [[ $? != 0 ]] && rc=1
done
exit $rc
