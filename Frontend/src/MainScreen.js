import { useEffect, useState } from "react";
import { apiGetChoices } from "./api/gameplay";
import { apiGetScoreboard, apiDeleteScoreboard, apiDeleteGameRecord } from "./api/scoreboard";
import GameScreen from "./gameplay/GameScreen"
import Scoreboard from "./scoreboard/Scoreboard";

export default function MainScreen({onLogout}) {
  const [shapes, setShapes] = useState([]);
  const [gameRecords, setGameRecords] = useState(null);

  useEffect(() => {
    if (!gameRecords)
        loadScoreboard()

    if (!shapes.length)
      loadChoices()
  });

  async function loadChoices() {
    const newChoices = await apiGetChoices();
    if (newChoices.success && JSON.stringify(newChoices.message) !== JSON.stringify(shapes))
      setShapes(newChoices.message);
  }

  async function loadScoreboard() {
    const newGameRecords = await apiGetScoreboard(10);
    if (!newGameRecords.success)
        return;

    setGameRecords(newGameRecords.message);
  }

  async function deleteRecords(id) {
    if (id)
      await apiDeleteGameRecord(id);
    else
      await apiDeleteScoreboard();

    await loadScoreboard();
  }

  return (
    <div className="rpssl-main">
      <div className="rpssl-header">
        <button className='rpssl-header-logout' onClick={onLogout}>Log out</button>
      </div>
      <div className="rpssl-body">
        <Scoreboard shapes={shapes} gameRecords={gameRecords} onDeleteRecords={deleteRecords}/>
        <GameScreen shapes={shapes} onGamePlayed={loadScoreboard}/>
      </div>
    </div>
  );
}
