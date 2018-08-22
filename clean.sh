#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOTDIR="$( cd "$DIR" && pwd )"

# Supports -q to make the cleanup commands run silently
options=':q'
while getopts $options option
do
	case $option in
		q ) QUIET=--quiet;;
	esac
done

OPTIONS="-d --force -x $QUIET"
EXCLUDES="--exclude=*.user --exclude=*.suo --exclude=*.sdf --exclude=*.opensdf --exclude=.vs"

git clean $OPTIONS $EXCLUDES "$ROOTDIR"
git submodule foreach --recursive "git clean $OPTIONS $EXCLUDES"
