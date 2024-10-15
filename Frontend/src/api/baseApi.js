export async function sendPostRequest(path, requestBody){
    console.log("Request body: " + JSON.stringify(requestBody))

    return await sendRequest(path, {
        method: "POST",
        body: JSON.stringify(requestBody),
        headers: {
            "Content-type": "application/json"
        }
    })
}

export async function sendDeleteRequest(path){
    return await sendRequest(path, {
        method: "DELETE"
    })
}

export async function sendRequest(path, options) {
    let response;
    try {
        response = await fetch(process.env.REACT_APP_API_URL + path, options)
    }
    catch(ex) {
        console.warn("Failed to reach the server, response code: " + ex)
        alert("Failed to reach the server!");
        return null;
    }

    if (!response.ok) {
      await handleErrorResponse(response);
      return null;
    }

    if (!isJsonContent(response.headers))
      return null;

    const responseBody = await response.json();
    console.log("Response body: " + JSON.stringify(responseBody));

    return responseBody;
}

async function handleErrorResponse(response) {
    let responseBody = null;
    let responseTitle = null;

    const isJsonResponse = isJsonContent(response.headers);
    if (isJsonResponse) {
      responseBody = await response.json();
      responseTitle = responseBody["title"];
    }

    console.log("Request failed! Response code: " + response.status +
                ", Response body: " + (responseBody ? JSON.stringify(responseBody) : "Empty"));
    alert("Failed to execute the action!" + (responseTitle ? "\n" + responseTitle : ""));
}

function isJsonContent(headers) {
    const contentType = headers.get("content-type");
    return contentType &&
          (contentType.indexOf("application/json") !== -1 || contentType.indexOf("application/problem+json") !== -1);
}
