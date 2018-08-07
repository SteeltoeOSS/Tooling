#!/usr/bin/env bash

basedir=$(dirname $0)/..
featureTestDir=$basedir/feature
rc=0

for project in $featureTestDir/*
do
    dotnet test $project
    [[ $? != 0 ]] && rc=1
done
exit $rc
