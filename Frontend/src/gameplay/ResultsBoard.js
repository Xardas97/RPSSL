import "./styles.css";

export default function ResultsBoard({playerShape, computerShape, result}) {
  if (!result){
    return (<div className="results-board"/>)
  }

  let resultMessage = "You ";
  let resultMessageColor;
  switch(result) {
    case 'lose':
      resultMessage += "LOST!";
      resultMessageColor = "red";
      break;
    case 'win':
      resultMessage += "WON!";
      resultMessageColor = "green";
      break;
    case 'tie':
      resultMessage += "TIED!";
      resultMessageColor = "orange";
      break;
  }

  return (
    <div className="results-board">
      <div className="results-column">
        <div>Player</div>
        <img className="chosen-shape" src={require('./images/' + playerShape + '.png')}/>
      </div>
      <div className="result-message" style={{color: resultMessageColor}}>{resultMessage}</div>
      <div className="results-column">
        <div>Computer</div>
        <img className="chosen-shape" src={require('./images/' + computerShape + '.png')}/>
      </div>
    </div>
  )
}
