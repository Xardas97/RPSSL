import ShapeImage from '../gameplay/ShapeImage';
import "./styles.css";

export default function ScoreItem({record, getShapeName}) {
  let resultMessage;
  let resultMessageColor;
  switch(record.results) {
    case 'lose':
      resultMessage = "LOSS";
      resultMessageColor = "red";
      break;
    case 'win':
      resultMessage = "WIN";
      resultMessageColor = "green";
      break;
    case 'tie':
      resultMessage = "TIE";
      resultMessageColor = "orange";
      break;
  }

  return (
    <div className="score-item">
      <div className="score-item-shape">
        <ShapeImage shape={getShapeName(record.player)}/>
      </div>
      <div className="result-message" style={{color: resultMessageColor}}>{resultMessage}</div>
      <div className="score-item-shape">
        <ShapeImage shape={getShapeName(record.computer)}/>
      </div>
    </div>
  )
}
