<?php
$username = $_POST["username"];

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

//$sql = "SELECT lose FROM player WHERE username ='".$username."'";
$sql = "UPDATE player SET win=(win+1),heart=(heart+100) WHERE username ='".$username."'";
$result = $conn->query($sql) or die("2 : query failed");

$conn->close();
?>
