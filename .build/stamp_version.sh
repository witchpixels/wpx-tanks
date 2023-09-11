#!/bin/bash

cat ./project.godot | sed "/^config\\/version=/s/=.*/=\"$GitVersion_FullSemVer\"/" > ./project.godot.2
cp ./project.godot.2 ./project.godot
rm ./project.godot.2
