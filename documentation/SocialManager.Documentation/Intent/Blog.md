# Blog
The blog component of SocialManager allows users to create and publish blog posts directly to their social media accounts. Users can write articles, add images and videos, and format their content using a rich text editor. The blog also supports scheduling posts for future publication, allowing users to plan their content in advance.

## Data structure
- BlogPost
  - id: string
  - title: string
  - content: string
  - short: string
  - markdown: string
  - author: string
  - tags: string[]
  - categories: string[]
  - createdAt: DateTime
  - updatedAt: DateTime
- SocialPost
  - id: string
  - blogPostId: string
  - content: string
  - platform: string
  - scheduledAt: DateTime
  - status: string
  - publishedAt: DateTime
- Media
  - id: string
  - blogPostId: string
  - url: string
  - type: string
  - altText: string
