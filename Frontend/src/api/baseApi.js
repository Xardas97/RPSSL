export async function sendPostRequest(path, requestBody, alertOnErrorResponse = false) {
  const body = JSON.stringify(requestBody);
  console.log("Request body: " + body)
  return await sendRequest(path, "POST", body, alertOnErrorResponse);
}

export async function sendDeleteRequest(path, alertOnErrorResponse = false) {
  return await sendRequest(path, "DELETE", null, alertOnErrorResponse);
}

export async function sendRequest(path, method = "GET", body = null, alertOnErrorResponse = false) {
  let response;
  try {
    response = await fetch(process.env.REACT_APP_API_URL + path, {
      method: method,
      credentials: 'include',
      headers: {
        "Content-type": "application/json"
      },
      body: body
    });
  }
  catch (ex) {
    console.warn("Failed to reach the server, response code: " + ex)
    alert("Failed to reach the server!");
    return null;
  }

  if (!response.ok) {
    const errorResponse = await handleErrorResponse(response, alertOnErrorResponse);
    return { success: false, message: errorResponse };
  }

  const responseBody = await getResponseBody(response);
  return { success: true, message: responseBody };
}

async function getResponseBody(response) {
  const contentType = getContentType(response.headers);
  switch (contentType) {
    case "json":
      const jsonResponseBody = await response.json();
      console.log("Response body: " + JSON.stringify(jsonResponseBody));
      return jsonResponseBody;
    case "plain":
      const plainResponseBody = await response.text();
      console.log("Response body: " + plainResponseBody);
      return plainResponseBody;
    default:
      return null;
  }
}

async function handleErrorResponse(response, alertOnError) {
  let responseBody = null;
  let responseTitle = null;

  const contentType = getContentType(response.headers);
  if (contentType === "json") {
    responseBody = await response.json();
    responseTitle = responseBody["title"];
  }

  console.log("Request failed! Response code: " + response.status + ", " +
              "Response body: " + (responseBody ? JSON.stringify(responseBody) : "Empty"));

  if (alertOnError)
    alert("Failed to execute the action!" + (responseTitle ? "\n" + responseTitle : ""));

  return responseTitle;
}

function getContentType(headers) {
  const contentType = headers.get("content-type");
  if (!contentType)
    return null;

  if (contentType.indexOf("application/json") !== -1 || contentType.indexOf("application/problem+json") !== -1)
    return "json";

  if (contentType.indexOf("text/plain") !== -1)
    return "plain";

  return null;
}
