import {sendRequest, sendPostRequest} from "./baseApi.js";

export const apiGetChoices = async () => {
    return await sendRequest("choices");
}

export const apiPostPlay = async (shape) => {
    const requestBody = { player: shape.id };
    return await sendPostRequest("play", requestBody)
}
