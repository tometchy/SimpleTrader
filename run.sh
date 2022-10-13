if [ -f "Docker/.env" ]; then
	sudo docker-compose -f Docker/docker-compose.yml up --build -d
else 
    echo "You must create Docker/.env file with proper variables first"
fi
