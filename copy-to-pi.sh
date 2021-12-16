if [ -z $PI_ADDRESS ]; then
    echo "PI_ADDRESS local variable missing"
else
    PI_ADDRESS=$PI_ADDRESS
    eval "rsync -av --exclude-from=exclude-from-pi.txt ./ ${PI_ADDRESS}:/home/pi/news-scrapper"
fi