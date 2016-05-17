# Catalog service
This project is a catalog servie in the flak.io microservices ecommerce sample application.

##Deployment
Elasticsearch for the catalog service can be deployed using DC/OS universe package.

Sample script to populate the cluster with soem data. Update the following address to use one of the elasticsearch nodes in the cluster.
```
curl https://raw.githubusercontent.com/flakio/catalog/master/sampledata/category1.json | curl -qs -XPOST 10.32.0.4:1025/products/category -d@- -H "Content-Type: application/json"
curl https://raw.githubusercontent.com/flakio/catalog/master/sampledata/category2.json | curl -qs -XPOST 10.32.0.4:1025/products/category -d@- -H "Content-Type: application/json"
curl https://raw.githubusercontent.com/flakio/catalog/master/sampledata/category3.json | curl -qs -XPOST 10.32.0.4:1025/products/category -d@- -H "Content-Type: application/json"

curl https://raw.githubusercontent.com/flakio/catalog/master/sampledata/product1.json | curl -qs -XPOST 10.32.0.4:1025/products/product -d@- -H "Content-Type: application/json"
curl https://raw.githubusercontent.com/flakio/catalog/master/sampledata/product2.json | curl -qs -XPOST 10.32.0.4:1025/products/product -d@- -H "Content-Type: application/json"
curl https://raw.githubusercontent.com/flakio/catalog/master/sampledata/product3.json | curl -qs -XPOST 10.32.0.4:1025/products/product -d@- -H "Content-Type: application/json"

```

##API
