# X (Twitter) API Setup Guide

## Overview
This guide walks you through obtaining X (Twitter) API credentials required for the SocialManager X Provider integration.

## Prerequisites
- An X (Twitter) account
- Access to the [X Developer Portal](https://developer.twitter.com/)

## Steps

### 1. Apply for a Developer Account

1. Navigate to https://developer.twitter.com/
2. Click **Sign up** or **Apply**
3. Fill out the application form:
   - **Use case**: Select "Making a bot" or describe your social media management use case
   - **Project description**: "Building a social media management tool"
   - Agree to the Developer Agreement and Policy
4. Verify your email address
5. Wait for approval (usually instant for basic access)

### 2. Create a Project and App

1. Log into the [X Developer Portal Dashboard](https://developer.twitter.com/en/portal/dashboard)
2. Click **Projects & Apps** ? **+ Create Project**
3. Enter project details:
   - **Name**: "SocialManager"
   - **Use Case**: Select appropriate category
   - **Description**: Brief description of your application
4. Click **Next** ? **+ Create App**
5. Enter **App Name**: "SocialManager X Integration"
6. Click **Complete**

### 3. Obtain API Keys and Tokens

#### API Key and Secret

1. Navigate to your app's **Keys and tokens** tab
2. Under **Consumer Keys**, copy:
   - **API Key** (Consumer Key)
   - **API Secret** (Consumer Secret)
   
?? **Important**: The API Secret is only shown once. Store it securely.

#### Access Token and Secret

1. Scroll to **Authentication Tokens**
2. Click **Generate** under "Access Token and Secret"
3. Copy both:
   - **Access Token**
   - **Access Token Secret**

?? **Important**: These tokens represent your Twitter account. Keep them confidential.

### 4. Configure App Permissions

1. Go to your app's **Settings** tab
2. Scroll to **User authentication settings**
3. Click **Set up** (or **Edit**)
4. Configure:
   - **App permissions**: Select **Read and write** (required for posting)
   - **Type of App**: Select **Web App, Automated App or Bot**
   - **Callback URI**: `http://localhost:3000/callback` (placeholder)
   - **Website URL**: Your application URL
5. Click **Save**

?? **Important**: After changing permissions, regenerate your Access Token and Secret.

## What You Need

After completing these steps, you should have:
- ? API Key (Consumer Key)
- ? API Secret (Consumer Secret)
- ? Access Token
- ? Access Token Secret
- ? App permissions set to "Read and write"

## Next Steps

See [Configuration.md](Configuration.md) for how to configure these credentials in your application.

## Rate Limits

Free tier limits:
- **Post Tweet**: 50 requests per 15 minutes, 1,500 tweets per month
- **Get Tweet**: 900 requests per 15 minutes

Consider upgrading to Basic ($100/month) or higher tiers for increased limits.

## Resources

- [X API Documentation](https://developer.twitter.com/en/docs)
- [Authentication Overview](https://developer.twitter.com/en/docs/authentication/overview)
- [Rate Limits](https://developer.twitter.com/en/docs/twitter-api/rate-limits)
