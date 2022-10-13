if [ -f "Docker/.env" ]; then
	sudo docker-compose -f Docker/docker-compose.yml build
else 
    echo "You must create Docker/.env file with proper variables first"
fi
