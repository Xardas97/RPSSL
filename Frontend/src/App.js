import { useEffect, useState } from "react";
import { apiGetChoices } from "./api/gameplay";
import { apiGetScoreboard, apiDeleteScoreboard, apiDeleteGameRecord } from "./api/scoreboard";
import GameScreen from "./gameplay/GameScreen"
import Scoreboard from "./scoreboard/Scoreboard";

export default function RPSSL() {
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
    if (newChoices && JSON.stringify(newChoices) !== JSON.stringify(shapes))
      setShapes(newChoices);
  }

  async function loadScoreboard() {
    const newGameRecords = await apiGetScoreboard(10);
    if (!newGameRecords)
        return;

    setGameRecords(newGameRecords);
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
      <Scoreboard shapes={shapes} gameRecords={gameRecords} onDeleteRecords={deleteRecords}/>
      <GameScreen shapes={shapes} onGamePlayed={loadScoreboard}/>
    </div>
  );
}
