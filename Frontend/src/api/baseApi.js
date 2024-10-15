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
        console.log("Failed to reach the server, response code: " + ex)
        alert("Failed to reach the server!");
        return null;
    }

    const contentType = response.headers.get("content-type");
    const isJsonResponse = contentType && contentType.indexOf("application/json") !== -1;

    if (!response.ok) {
      let responseBody = null;
      if (isJsonResponse)
        responseBody = await response.json();

      console.log("Request failed! Response code: " + response.status + ", Response body: " + JSON.stringify(responseBody));

      const responseAlert = responseBody["title"] ? "\n" + responseBody["title"] : "";
      alert("Failed to execute the action!" + responseAlert);
      return null;
    }

    if (!isJsonResponse)
        return null;

    const responseBody = await response.json();
    console.log("Response body: " + JSON.stringify(responseBody));

    return responseBody;
}
