#!/bin/bash

# wget -qO - http://deb.opera.com/archive.key | sudo apt-key add -

wget -qO - https://packagecloud.io/headmelted/codebuilds/gpgkey | sudo apt-key add -
#sudo apt update
apt-key list
su
. <( wget -O - https://code.headmelted.com/installers/apt.sh )

