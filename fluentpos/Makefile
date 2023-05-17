build:
	@dotnet build /ll

tye:
	@tye run

docker-up:
	@docker-compose -f docker-compose.infrastructure.yml -f docker-compose.yml pull
	@docker-compose -f docker-compose.infrastructure.yml -f docker-compose.yml up -d

docker-down:
	@docker-compose -f docker-compose.infrastructure.yml -f docker-compose.yml down

dotnet-publish:
	@cd ./gateways/gateway && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./services/catalog/api && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./services/identity/api && dotnet publish --os linux --arch x64 -c Release --self-contained
	@cd ./services/cart/api && dotnet publish --os linux --arch x64 -c Release --self-contained