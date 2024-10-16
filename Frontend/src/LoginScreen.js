import { useEffect, useState } from "react";
import { apiLogin, apiRegister } from "./api/login"

export default function LoginScreen() {
  const [errorMessage, setErrorMessage] = useState(null)
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')

  async function login() { await submit(apiLogin); }
  async function register() { await submit(apiRegister); }
  async function submit(apiSubmit) {
    setErrorMessage(null)

    const response = await apiSubmit(username, password);
    if (!response.success)
      setPassword('');
      setErrorMessage(response.message)
  }

  useEffect(() => {
    const keyboardListener = async (event) => {
      // Do login on user pressing enter
      if (event.code === "Enter" || event.code === "NumpadEnter")
        await login();
    };

    document.addEventListener("keydown", keyboardListener);
    return () => {
      // Remove the listener when moved away from login screen
      document.removeEventListener("keydown", keyboardListener);
    };
  });

  return (
    <div className="rpssl-login">
      <div className="login-row">
        <label className="login-error-message">{errorMessage}</label>
      </div>
      <input className="login-row" type="text" placeholder="username" value={username} onChange={(e) => setUsername(e.target.value)} />
      <input className="login-row" type="password" placeholder="password" value={password} onChange={(e) => setPassword(e.target.value)} />
      <div className="login-row" >
        <button className="login-button" onClick={login}>Login</button>
        <button className="login-button"  onClick={register}>Register</button>
      </div>
    </div>
  );
}
