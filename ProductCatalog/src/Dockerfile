FROM microsoft/aspnet:1.0.0-rc1-update1

ENV server.urls="http://0.0.0.0:5000"

COPY . /app

WORKDIR /app/ProductCatalog

RUN ["dnu", "restore"]

CMD ["dnx", "-p", "project.json", "web"]
