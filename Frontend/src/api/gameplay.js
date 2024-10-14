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
    const response = await fetch(process.env.REACT_APP_API_URL + path, options)

    if (response.status != 200){
      console.log("Failed to reach the server, response code: " + response.status)
      return null;
    }

    const responseBody = await response.json();
    console.log("Response body: " + JSON.stringify(responseBody));

    return responseBody;
}