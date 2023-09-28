# Stage Two

<details>
<summary><b>Objectives</b></summary>

You are to build a simple REST API capable of CRUD operations on a "person" resource, interfacing with any database of your choice. 

Your API should dynamically handle parameters, such as adding or retrieving a person by name. Accompany the development with UML diagrams to represent your system's design and database structure. 

Host your entire project on GitHub, and provide a well-structured documentation in the repository that outlines request/response formats, setup instructions, and sample API usage.

### Breakdown:

1.  REST API Development:

    Develop an API with endpoints for:

    - CREATE: Adding a new person.  => `/api`
    - READ: Fetching details of a person.  => `/api/user_id`
    - UPDATE: Modifying details of an existing person => `/api/user_id`
    - DELETE: Removing a person => `/api/user_id`
    
    Ensure all interactions with the database are secure and free from common vulnerabilities (e.g., SQL injections).

2.  Database Modelling: (Bonus)

    - UML Diagram: 
    
      Design and present a UML (Unified Modeling Language) diagram that represents the structure and relationships of your API's classes and models.

3.  Testing:

    Using tools like Postman or (scripts written in Python using the requests library) that tests each CRUD operation in your API.
    
    This should:
    
    - Add a new person (e.g., "Mark Essien").
    - Fetch details of a person
    - Modify the details of an existing person.
    - Remove a person 

4.  Dynamic Parameter Handling:

    - Your API should be flexible enough to handle dynamic input. 
      
      If we provide a name (or other details), your backend should be able to process operations using that name.

    - Example: If we pass "Mark Essien", we should be able to perform all CRUD operations on "Mark Essien".
    
    - Add validation – field should only be strings; integers or any other data type should not be allowed.

5.  GitHub Repository:

    Create a GitHub repository for this project.

    Ensure the repository contains:
    
    - A detailed README.md file explaining how to set up, run, and use the API.
    
    - The source code for the API.
    
    - UML diagrams (or links to view them).

6.  Documentation:

    Provide a documentation file (e.g., DOCUMENTATION.md in your GitHub repo) that outlines:
    
    - Standard formats for requests and responses for each endpoint.
    
    - Sample usage of the API, including example requests and expected responses.
    
    - Any known limitations or assumptions made during development.
    
    - Instructions for setting up and deploying the API locally or on a server.

7.  Hosting

    - Using the same Server used in the Stage One task (or another server, if possible), modify it accordingly to  host your endpoint with a URL like this `https://theirdomain.com/api`

    - Test extensively with various testing tools to make sure it is accessible before submitting

### Acceptance Criteria

- Functional REST API: The API should successfully perform all CRUD operations.

- Modelling Diagrams: Clear and accurate UML and E-R diagrams should be provided.

- Effective Testing Script: The script should successfully test all the CRUD operations without manual intervention.

- Dynamic Parameter Handling: The API should correctly handle and respond to different parameters provided.

- GitHub Repository: Repository should be well-organized, contain all necessary files, and be publicly accessible.

- Detailed Documentation: Documentation should provide clear guidance on how to use the API, including setup, request/response formats, and sample usage.

</details>

Live demo is at [https://hngx-stage-two-c.onrender.com/api](https://hngx-stage-two-c.onrender.com/api)

Run it like:

```sh
curl -X 'POST' \
    'https://hngx-stage-two-c.onrender.com/api' \
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

You can read more in the [Documentation](DOCUMENTATION.md).

## Testing

A collection (29716394-8b5069b1-6ce0-42ba-a3ca-a6cd203ca804) has been provided with tests, run it using [Postman CLI](https://learning.postman.com/docs/postman-cli/postman-cli-installation) like:

`postman collection run 29716394-8b5069b1-6ce0-42ba-a3ca-a6cd203ca804 --global-var "base_api=<url-to-test>"`. 

For example, to test the live link:

```sh
$ postman collection run 29716394-8b5069b1-6ce0-42ba-a3ca-a6cd203ca804 --global-var "base_api=https://hngx-stage-two-c.onrender.com/api"
```

A copy of the collection can be found at [Collections\hngx-duo.postman_collection.json](Collections/hngx-duo.postman_collection.json).

## Build locally

- Clone the repository:

```sh
$ git clone https://github.com/poopsicles/HNGx-dotnet.git
$ cd HNGx-dotnet/stage-two
```

- Build the image:

```sh
$ docker build . --pull -t hngx-duo/dotnet
```

- Run the container:

```sh
$ docker run --rm -it -p 8000:80 hngx-duo/dotnet
```

It will be running at `http://localhost:8000`