# Person API

This API allows you to store, retrieve, modify and delete `Person`s.

A `Person` is defined as follows:

```csharp
{
    "id" : Guid
    "name": String,
    "age": Integer,
    "favouriteColour": String
}
```

The ERD and UML class diagrams can be seen at [DIAGRAMS.md](DIAGRAMS.md).

`favouriteColour` can be any one of the following values:

```json
[ "Red", "Green", "Blue", "Yellow", "Orange", "Purple", "Black" ]
```

`age` must be an integer greater than 0.

`name`s must be unique, but are case-sensitive, therefore:

```csharp
{
    "id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    "name": "David",
    "age": 10,
    "favouriteColour": "Red"
}
```

and 

```csharp
{
    "id" : "f24b5270-dd28-4897-bb50-18d6ff65798e"
    "name": "david",
    "age": 10,
    "favouriteColour": "Red"
}
```

are two different entities, and as such, would have different IDs.

## Endpoints

The API has four defined endpoints:

| Method |   Route   |        Description        | Successful Status Code |
|:------:|:---------:|:-------------------------:|:----------------------:|
|  POST  |    /api   |     Creates a `Person`    |           201          |
|   GET  | /api/{id} |   Gets a `Person` by ID   |           200          |
|  PATCH | /api/{id} | Modifies a `Person` by ID |           200          |
| DELETE | /api/{id} |  Deletes a `Person` by ID |           204          |

More details can be found below.

- **POST** `/api` - Creates a `Person` with the values specified in the body

  The request body contains mandatory parameters that signify what the `name`, `age` or `favouriteColour` should be in the new entity. You must include all of these parameters.

  When successful, the created entity is returned, which contains the attributes as well as its `id`.

  ```sh
  curl -X 'POST' \
    'http://localhost/api' \
    -H 'accept: application/json' \
    -H 'Content-Type: application/json' \
    -d '{
    "name": "Mary",
    "age": 10,
    "favouriteColour": "red"
  }'
  ```
  ```json
  201 Created
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Mary",
    "age": 10,
    "favouriteColour": "Red"
  }
  ```

  The body parameters are validated to ensure they conform to the rules, and if they fail, the errors are returned in the following format:

  ```sh
  curl -X 'POST' \
    'http://localhost/api' \
    -H 'accept: application/json' \
    -H 'Content-Type: application/json' \
    -d '{
    "name": "Already-existing-name",
    "age": -1,
    "favouriteColour": "not-a-colour"
  }'
  ```
  ```json
  422 Unprocessible Content
  "{
    "errors": [
      "Age cannot be zero or negative",
      "Invalid colour",
      "Duplicate name"
    ]
  }"
  ```

- **GET** `/api/{id}` - Retrieves the details of a `Person` with `id`

  `id` is a mandatory path parameter that signifies the entity to get the details of.

  When successful, i.e. the `id` points to valid entity in the database, the response will be in this format:

  ```sh
  curl -X 'GET' \
    'http://localhost/api/3fa85f64-5717-4562-b3fc-2c963f66afa6' \
    -H 'accept: application/json'
  ```
  ```json
  200 OK
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "John Smith",
    "age": 42,
    "favouriteColour": "Blue"
  }
  ```

  If `id` is invalid, the request will fail and return a response like:

  ```sh
  curl -X 'GET' \
    'http://localhost/api/not-a-uuid' \
    -H 'accept: application/json'
  ```
  ```json
  404 Not Found
  {
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "traceId": "00-10fb7769e20daccf8a09bf8205acdfd5-5501ebcf22255b7b-00"
  }
  ```

- **PATCH** `/api/{id}` - Modifies the details of a `Person` with `id` with the values specified in the body

  `id` is a mandatory path parameter that signifies the entity to modify.

  The request body contains optional parameters that signify what the `name`, `age` or `favouriteColour` should be changed to. You can include any mix of these parameters.
  
  For example:

  ```sh
  curl -X 'PATCH' \
    'http://localhost/api/3fa85f64-5717-4562-b3fc-2c963f66afa6' \
    -H 'accept: application/json' \
    -H 'Content-Type: application/json' \
    -d '{
    "name": "Mary",
    "age": 10,
    "favouriteColour": "red"
  }'
  ```
  and
  ```sh
  curl -X 'PATCH' \
    'http://localhost/api/3fa85f64-5717-4562-b3fc-2c963f66afa6' \
    -H 'accept: application/json' \
    -H 'Content-Type: application/json' \
    -d '{
    "name": "Mary",
  }'
  ```

  are both valid PATCH requests that will both change the entity with `id` *3fa85f64-5717-4562-b3fc-2c963f66afa6*'s name to "Mary", along with the first also changing the `age` and `favouriteColour`.

  When successful, the entire updated entity is returned:
  ```json
  200 OK
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Mary",
    "age": 10,
    "favouriteColour": "Red"
  }
  ```

  The body parameters are validated to ensure they conform to the rules, and if they fail, the errors are returned in the following format:

  ```sh
  curl -X 'PATCH' \
    'http://localhost/api/3fa85f64-5717-4562-b3fc-2c963f66afa6' \
    -H 'accept: application/json' \
    -H 'Content-Type: application/json' \
    -d '{
    "name": "Already-existing-name",
    "age": -1,
    "favouriteColour": "not-a-colour"
  }'
  ```
  ```json
  422 Unprocessible Content
  "{
    "errors": [
      "Age cannot be zero or negative",
      "Invalid colour",
      "Duplicate name"
    ]
  }"
  ```

- **DELETE** `/api/{id}` - Deletes a `Person` with `id`

  `id` is a mandatory path parameter that signifies the entity to modify.
  
  When successful, i.e. the `id` points to valid entity in the database, the entity is deleted and no content is returned:
  
    ```sh
  curl -X 'DELETE' \
    'http://localhost/api/3fa85f64-5717-4562-b3fc-2c963f66afa6' \
    -H 'accept: application/json'
  ```
  ```json
  204 No Content
  {}
  ```

  If `id` is invalid, the request will fail and return a response like:

  ```sh
  curl -X 'GET' \
    'http://localhost/api/not-a-uuid' \
    -H 'accept: application/json'
  ```
  ```json
  404 Not Found
  {
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "traceId": "00-10fb7769e20daccf8a09bf8205acdfd5-5501ebcf22255b7b-00"
  }
  ```

## Appendix:

- Primary keys, `Person.id` are version 4 UUIDs, chosen for their randomness.

- The database chosen is SQLite, which limits possible types that can be stored to text and numbers; UUIDs here are converted to text and offer no type safety, therefore the client isn't allowed to generate them - they're generated server-side.

- Internally, `favouriteColour` is stored as an enum.