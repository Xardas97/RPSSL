export const apiGetChoices = async () => {
    return await sendRequest("choices");
}

export const apiPostPlay = async (shape) => {
    const requestBody = { player: shape.id };
    return await sendPostRequest("play", requestBody)
}

async function sendPostRequest(path, requestBody){
    console.log("Request body: " + JSON.stringify(requestBody))

    return await sendRequest(path, {
        method: "POST",
        body: JSON.stringify(requestBody),
        headers: {
            "Content-type": "application/json"
        }
    })
}

async function sendRequest(path, options) {
    let response;
    try {
        response = await fetch(process.env.REACT_APP_API_URL + path, options)
    }
    catch(ex) {
        console.log("Failed to reach the server, response code: " + ex)
        alert("Failed to reach the server!");
        return null;
    }

    if (response.status != 200) {
      const responseBody = await response.json();
      console.log("Request failed! Response code: " + response.status + ", Response body: " + JSON.stringify(responseBody));

      const responseAlert = responseBody["title"] ? "\n" + responseBody["title"] : "";
      alert("Failed to execute the action!" + responseAlert);
      return null;
    }

    const responseBody = await response.json();
    console.log("Response body: " + JSON.stringify(responseBody));

    return responseBody;
}