Github review bot

Starting: docker build . -t github_bot &&  docker run -p 6060:80 -e ApikKey=your_api_key -v DB:/app/DB/ github_bot