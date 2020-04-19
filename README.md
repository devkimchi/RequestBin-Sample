# Durable RequestBin Sample #

This shows a code sample to emulate RequestBin, using Azure Durable Functions


## Prerequisites ##

* Azure Subscription: [Get a free account today](https://azure.microsoft.com/free/?WT.mc_id=devkimchicom-github-juyoo)


## Getting Started ##

### Clone Repository ###

* Clone this repository to your local.

```bash
git clone https://github.com/devkimchi/RequestBin-Sample.git
```


### App Settings ###

Update `local.settings.json` with the storage account connection string.

* `AzureWebJobsStorage`: Get the connection string of your preferred [Azure Storage Account](https://docs.microsoft.com/azure/storage/common/storage-introduction?WT.mc_id=devkimchicom-github-juyoo)



## Endpoints ##

There are five different endpoints on this function app.

### Create Bin: `/bins` ###

This creates a new bin to take all requests.

```bash
curl -X POST -H "Content-Length: 0" "http://localhost:7071/bins"
```

The response might look like, with the HTTP status code of `201 (Created)`:

```json
{
  "bin": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646",
  "history": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/history",
  "reset": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/reset",
  "purge": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/purge"
}
```


### Add Request to Bin: `/bins/<bin-id>` ###

This sends requests to bin. VERB can be `GET`, `POST`, `PUT`, `PATCH`, `DELETE`, etc.

```bash
curl -X POST -H "Authorization: Basic 12345" -H "Content-Type: application/json" "http://localhost:7071/bins/<bin-id>" -d '{ "hello": "world" }'
```

The response might look like, with the HTTP status code of `202 (Accepted)`:

```json
{
  "bin": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646",
  "history": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/history",
  "reset": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/reset",
  "purge": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/purge"
}
```

### List Requests from Bin: `/bins/<bin-id>/history` ###

This gets the request history.

```bash
curl -X GET "http://localhost:7071/bins/<bin-id>/history"
```

The response might look like, with the HTTP status code of `200 (OK)`:

```json
{
  "history": [
    {
      "timestamp": "2020-04-19T07:36:19.082+00:00",
      "method": "POST",
      "headers": {
        // LIST OF HEADERS
      },
      "queries": {
        // LIST OF QUERIES
      },
      "body": "<REQUEST_PAYLOAD>"
    }
  ],
  "navigation": {
    "bin": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646",
    "history": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/history",
    "reset": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/reset",
    "purge": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/purge"
  }
}
```


### Reset Bin: `/bins/<bin-id>/reset` ###

This clears the request history from the bin.

```bash
curl -X DELETE "http://localhost:7071/bins/<bin-id>/reset"
```

The response might look like, with the HTTP status code of `202 (Accepted)`:

```json
{
  "bin": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646",
  "history": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/history",
  "reset": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/reset",
  "purge": "https://fncapp-requestbin-krc.azurewebsites.net/bins/b2b2ff35-c303-46dd-864a-0a2353de3646/purge"
}
```


### Delete Bin: `/bins/<bin-id>/purge` ###

This removes the bin itself.

```bash
curl -X DELETE "http://localhost:7071/bins/<bin-id>/purge"
```

The response returns nothing but the HTTP status code of `204 (No content)`


## Contribution ##

Your contributions are always welcome! All your work should be done in your forked repository. Once you finish your work with corresponding tests, please send us a pull request onto our `master` branch for review.


## License ##

This is released under [MIT License](http://opensource.org/licenses/MIT)

> The MIT License (MIT)
>
> Copyright (c) 2019 [Dev Kimchi](https://devkimchi.com)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
