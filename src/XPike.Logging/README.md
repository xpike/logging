# xPike.Logging

#### Why log things?

Because sometimes you need to see what happened.

#### Advice...

- **Logs should be useful.**
- Don't log things which aren't useful, or in a useless way.
- **Errors are severe - as in PagerDuty.**
- Things that aren't severe are just warnings.
- **Logged Exceptions are at least warnings.**
- If it's not concerning, just log a message - or don't log at all.
- **Info is important.**
- This should be your "default" log level when viewing logs.
- **Logs ***should*** be useful.**
- For things you **need** to log but don't care about, use the `Log` level.
- **There's Debug and Trace also...**
- If you don't know it will be useful, that's for `Trace`.