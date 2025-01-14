import { useState } from 'react';
import { apiPostPlay } from '../api/gameplay';
import ChoiceBoard from "./ChoiceBoard"
import ResultsBoard from "./ResultsBoard"
import "./styles.css";

export default function GameScreen({shapes, onGamePlayed}) {
  const [result,  setResult] = useState(null);
  const [playerShape, setPlayerShape] = useState(null);
  const [computerShape, setComputerShape] = useState(null);
  const [gameInProgress, setGameInProgress] = useState(false);

  async function playShape(shape) {
    function getShapeName(shapeId) {
      return shapes.find(s => s.id == shapeId).name
    }

    if (gameInProgress)
      return;

    setResult(null);
    setPlayerShape(null);
    setComputerShape(null);

    setGameInProgress(true);
    const response = await apiPostPlay(shape);
    setGameInProgress(false);

    if (!response.success)
      return;

    const playerShape = getShapeName(response.message.player);
    const computerShape = getShapeName(response.message.computer);

    setResult(response.message.results);
    setPlayerShape(playerShape);
    setComputerShape(computerShape);

    onGamePlayed();
  }

  return (
    <div className="game">
      <ChoiceBoard shapes={shapes} disable={gameInProgress} onPlayShape={playShape} />
      <ResultsBoard playerShape={playerShape} computerShape={computerShape} result={result} gameInProgress={gameInProgress} />
    </div>
  );
}
