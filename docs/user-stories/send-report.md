# User Story: Automate Report Sending

## Description
As an user (admin), I want the system to automatically send out reports on a configurable schedule, so that recipients receive updates without requiring manual intervention.

## Acceptance Criteria
- The user (admin) can configure the report sending frequency (e.g., daily, weekly, monthly) via a settings page. 
- At the scheduled time, the system automatically generates the report and sends it to all recipients email addresses. The list of recipients is already defined elsewhere.
- If the email sending fails, the system logs an error.
- There is an option for the user to manually trigger the report sending if needed.

## Notes
- If the email sending fails, user should be notified about it (how?).

## Estimation
Story points: 8