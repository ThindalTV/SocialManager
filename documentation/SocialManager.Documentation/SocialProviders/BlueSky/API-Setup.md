# BlueSky API Setup Guide

## Overview
This guide walks you through setting up a BlueSky account and generating app passwords required for the SocialManager BlueSky Provider integration.

## Prerequisites
- A BlueSky account (https://bsky.app)
- Access to BlueSky settings

## Steps

### 1. Create a BlueSky Account

1. Navigate to https://bsky.app
2. Click **Sign up** or download the mobile app
3. Choose your username (handle):
   - Format: `username.bsky.social`
   - Example: `alice.bsky.social`
4. Complete the registration process
5. Verify your email address

### 2. Generate an App Password

App passwords are required for third-party applications to access your BlueSky account via the AT Protocol.

1. Log into BlueSky at https://bsky.app
2. Navigate to **Settings** ? **Privacy and security**
3. Scroll to **App passwords** section
4. Click **Add app password**
5. Enter a name for this password:
   - Recommended: "SocialManager Integration"
   - This helps identify which app is using the password
6. Click **Create App Password**
7. **Copy the generated password immediately**
   - The password is shown only once
   - It will look like: `xxxx-xxxx-xxxx-xxxx`

?? **Important**: Store the app password securely. You won't be able to see it again.

### 3. Identify Your PDS Endpoint

For most users, the default PDS (Personal Data Server) endpoint is sufficient:
- **Default PDS**: `https://bsky.social`

If you're using a custom PDS or self-hosted instance:
1. Check your BlueSky profile settings
2. Look for "PDS Host" or similar setting
3. Note the custom URL (e.g., `https://your-pds.example.com`)

### 4. Understanding AT Protocol Authentication

BlueSky uses the AT Protocol (Authenticated Transfer Protocol) for authentication:

- **Identifier**: Your handle (e.g., `alice.bsky.social`) or DID
- **App Password**: The password generated in step 2
- **PDS URL**: Your Personal Data Server endpoint

The authentication flow:
1. Application connects to PDS endpoint
2. Authenticates using identifier + app password
3. Receives a session token for subsequent requests
4. Token is automatically refreshed as needed

## What You Need

After completing these steps, you should have:
- ? BlueSky handle (identifier)
- ? App password
- ? PDS endpoint URL (default: `https://bsky.social`)

## Security Considerations

### App Password Management

1. **One password per application**
   - Generate separate app passwords for each integration
   - Makes it easier to revoke access if needed

2. **Regular rotation**
   - Consider rotating app passwords every 90 days
   - Update configuration when rotating

3. **Immediate revocation**
   - If compromised, revoke immediately from BlueSky settings
   - Generate a new password and update configuration

### Revoking App Passwords

To revoke an app password:
1. Go to **Settings** ? **Privacy and security**
2. Find the app password in the list
3. Click **Delete** or **Revoke**
4. The app password will immediately stop working

## Next Steps

See [Configuration.md](Configuration.md) for how to configure these credentials in your application.

## Rate Limits

BlueSky enforces rate limits on API requests:

- **Posts**: Approximately 50 posts per 15 minutes
- **Reads**: More generous, typically 300-500 per 15 minutes
- **Media uploads**: Subject to size limits (5MB per image)

Rate limits may vary and are subject to change. Monitor your application logs for rate limit errors.

## Resources

- [BlueSky Official Site](https://bsky.app)
- [AT Protocol Documentation](https://atproto.com)
- [BlueSky API Docs](https://docs.bsky.app)
- [AT Protocol Specs](https://atproto.com/specs/atp)

## Troubleshooting

### Can't Find App Passwords Setting

- Ensure you're logged into the web interface at https://bsky.app
- App password creation may not be available in mobile apps
- Check that your account is fully verified

### App Password Not Working

- Verify you copied the entire password (including all dashes)
- Check for extra spaces at the beginning or end
- Ensure you're using the correct identifier (handle)
- Confirm the PDS URL is correct

### "Invalid Credentials" Error

- Double-check your handle format (e.g., `username.bsky.social`)
- Verify the app password hasn't been revoked
- Try generating a new app password
- Ensure there are no typos in either field

## See Also

- [Configuration.md](Configuration.md) - Configure credentials in your application
- [Usage-Examples.md](Usage-Examples.md) - Code examples
- [Troubleshooting.md](Troubleshooting.md) - Common issues and solutions
