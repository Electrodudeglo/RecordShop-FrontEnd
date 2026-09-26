# RecordShop Front End

A Blazor (.NET 8) front end for a record shop. You can browse the records, search them, and view each one. After you log in you can also add, edit and delete records. New albums are looked up on Deezer.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A running RecordShop backend API. You can use [Electrodudeglo/RecordShop](https://github.com/Electrodudeglo/RecordShop) as an example.

## Getting started

1. **Start the backend.** Clone and run the example backend (follow the instructions in its README):

   ```bash
   git clone https://github.com/Electrodudeglo/RecordShop.git
   ```

2. **Point the front end at the backend.** Open `RecordShop-FrontEnd/RecordShop-FrontEnd/appsettings.json` and set `ApiBaseUrl` to your backend's address:

   ```json
   "ApiBaseUrl": "http://localhost:5125/"
   ```

3. **Run the front end.**

   ```bash
   cd RecordShop-FrontEnd/RecordShop-FrontEnd
   dotnet run
   ```

4. **Open it** at http://localhost:5263 in your browser.

## Using the app

- **Browse:** the home page lists every record. Use the search bar to filter them.
- **Log in:** open the login page to sign in with a user from your backend.
- **Manage records:** once you are logged in, you can add a record (it is checked against Deezer first), edit it, or delete it.

## Backend endpoints used

The front end needs a backend that provides these endpoints:

| Method | Endpoint | Purpose |
| ------ | -------- | ------- |
| POST | `api/auth/token` | Log in; returns `{ "token": "..." }` |
| POST | `api/auth/logout` | Log out |
| GET | `api/v1/records` | Get all records |
| GET | `api/v1/records/{id}` | Get one record |
| POST | `api/v1/records/check-deezer` | Look up an album on Deezer |
| POST | `api/v1/records` | Add a record |
| PUT | `api/v1/records/{id}` | Update a record |
| DELETE | `api/v1/records/{id}` | Delete a record |

Add, update, delete and the Deezer lookup send the login token as a `Bearer` header.
