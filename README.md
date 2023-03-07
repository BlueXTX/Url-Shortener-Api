# Url Shortener Api 
[![Build and test](https://github.com/BlueXTX/Url-Shortener-Api/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/BlueXTX/Url-Shortener-Api/actions/workflows/dotnet-test.yml)
[![codecov](https://codecov.io/github/BlueXTX/Url-Shortener-Api/branch/master/graph/badge.svg?token=2ULS7LHKPO)](https://codecov.io/github/BlueXTX/Url-Shortener-Api)
### Builds and Packages
---
#### Build from source:
``` 
git clone https://github.com/BlueXTX/Url-Shortener-Api
dotnet build ./src/ShortUrl.Api
```

#### Docker build: 
```
docker build -t bluextx/url-shortener-api .
```

#### Docker run:
```
docker run -d -p 8080:80 -e ASPNET_ENVIRONMENT=Development ghcr.io/bluextx/url-shortener-api:master
```

### Api Documentation
---
OpenApi documentation is available at ```/swagger``` endpoint
