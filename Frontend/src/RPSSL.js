import { useCookies } from "react-cookie";
import MainScreen from "./MainScreen";
import LoginScreen from "./LoginScreen"

export default function RPSSL() {
  const [cookies,, removeCookie] = useCookies(['Authorization'])

  function onLogout() {
    removeCookie('Authorization', {path:'/'});
  }

  const screen = cookies.Authorization ? <MainScreen onLogout={onLogout}/>
                                       : <LoginScreen/>
  return (
    <div className="rpssl">
      {screen}
    </div>
  );
}
