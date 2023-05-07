build:
	@dotnet build

tye:
	@tye run

docker-up:
	@cd ./deployments/docker-compose && docker-compose -f infrastructure.yml -f docker-compose.yml pull
	@cd ./deployments/docker-compose && docker-compose -f infrastructure.yml -f docker-compose.yml up -d

docker-down:
	@cd ./deployments/docker-compose && docker-compose -f infrastructure.yml -f docker-compose.yml down

dotnet-publish:
	@cd ./gateways/gateway && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./services/catalog/api && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./services/identity/api && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./services/cart/api && dotnet publish --os linux --arch x64 -c Release --self-contained