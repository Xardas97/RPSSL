import { useState, useEffect } from 'react';
import { apiGetChoices, apiPostPlay } from '../api/gameplay';
import ChoiceBoard from "./ChoiceBoard"
import ResultsBoard from "./ResultsBoard"
import "./styles.css";

export default function GameScreen() {
  const [shapes, setShapes] = useState([]);
  const [result,  setResult] = useState(null);
  const [playerShape, setPlayerShape] = useState(null);
  const [computerShape, setComputerShape] = useState(null);

  useEffect(() => {
    if (!shapes.length)
      loadChoices()
  });

  async function loadChoices() {
    const newChoices = await apiGetChoices();
    if (newChoices && JSON.stringify(newChoices) !== JSON.stringify(shapes))
      setShapes(newChoices);
  }

  async function playShape(shape) {
    function getShapeName(shapeId) {
      return shapes.find(s => s.id == shapeId).name
    }

    setResult(null);
    setPlayerShape(null);
    setComputerShape(null);

    console.log("Playing with: " + shape.name);
    const response = await apiPostPlay(shape);
    if (!response)
      return;

    const playerShape = getShapeName(response.player)
    const computerShape = getShapeName(response.computer)

    setResult(response.results);
    setPlayerShape(playerShape);
    setComputerShape(computerShape);
  }

  return (
    <div className="game">
      <ChoiceBoard shapes={shapes} playShape={playShape} />
      <ResultsBoard playerShape={playerShape} computerShape={computerShape} result={result}/>
    </div>
  );
}