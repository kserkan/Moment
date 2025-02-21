# Moment Project

## General Information

- **Project Name:** Moment
- **Developer:** Kerim Serkan Şahin
- **Purpose:**
  - A social media platform that allows users to share memories, upload photos, and interact with other users.
  - Provides a fast and secure experience with a user-friendly interface and robust backend.
  - Supports social media interactions such as photo uploads, likes, comments, and following other users.
  - Secure session management with JWT authentication.

## Technologies Used

### Backend
- **.NET Core, ASP.NET MVC** (Layered architecture, performance, and maintainability)
- **Entity Framework Core** (ORM for database operations)
- **SQL Server** (Database management)
- **Web API** (RESTful services, JSON data format)
- **Docker** (Independence and easy deployment)
- **JWT Authentication** (Secure authentication)

### Frontend
- **HTML, CSS, Bootstrap** (Responsive design)
- **JavaScript** (Dynamic content management)

## Database Structure

- **User - Photo (1-N):** Users can upload multiple photos.
- **User - Comment (1-N):** Users can make multiple comments.
- **User - Like (1-N):** Users can like photos.
- **User - Follower (N-N):** Users can follow each other.
- **User - Notification (1-N):** Users can receive notifications.

## Application Workflow

### User Section
1. **Sign Up:** Users register by entering their name, email, and password.
2. **Login:** Users log in with their email and password. A JWT token is generated.
3. **Photo Upload:** Users can upload photos, add a title, and a description.
4. **Interactions:** Liking photos, commenting, following users, and receiving notifications.

### API Section
- **Authentication:** User sessions are managed with JWT.
- **Photo Management:** Uploading, deleting, and listing photos.
- **User Interactions:** Follow, comment, and like operations.
- **Notification System:** Real-time notifications.

## License
This project is licensed under the MIT License.
