build:
	dotnet build
tye:
	tye run

infra:
	cd ./deployments/docker-compose && docker-compose -f infrastructure.yml up -d

services:
	cd ./deployments/docker-compose && docker-compose -f docker-compose.yml up -d

all-up:
	cd ./deployments/docker-compose && docker-compose -f infrastructure.yml -f docker-compose.yml pull
	cd ./deployments/docker-compose && docker-compose -f infrastructure.yml -f docker-compose.yml up -d
all-down:
	cd ./deployments/docker-compose && docker-compose -f infrastructure.yml -f docker-compose.yml down