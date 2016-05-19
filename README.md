# Catalog service
This project is a catalog servie in the flak.io microservices ecommerce sample application.

## Deployment
Elasticsearch for the catalog service can be deployed using DC/OS universe package.


```
curl https://raw.githubusercontent.com/flakio/catalog/master/marathon.json | curl -qs -XPOST localhost/marathon/v2/apps -d@- -H "Content-Type: application/json"
```

##API
