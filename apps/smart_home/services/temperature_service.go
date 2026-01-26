package services

import (
	"bytes"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"net/url"
	"path"
	"time"
)

// TemperatureService handles fetching temperature data from external API
type TemperatureService struct {
	BaseURL        string
	IntegrationURL string
	HTTPClient     *http.Client
}

// TemperatureResponse represents the response from the temperature API
type TemperatureResponse struct {
	Value       float64   `json:"value"`
	Unit        string    `json:"unit"`
	Timestamp   time.Time `json:"timestamp"`
	Location    string    `json:"location"`
	Status      string    `json:"status"`
	SensorID    string    `json:"sensor_id"`
	SensorType  string    `json:"sensor_type"`
	Description string    `json:"description"`
}

// NewTemperatureService creates a new temperature service
func NewTemperatureService(baseURL string, integrationUrl string) *TemperatureService {
	return &TemperatureService{
		BaseURL:        baseURL,
		IntegrationURL: integrationUrl,
		HTTPClient: &http.Client{
			Timeout: 10 * time.Second,
		},
	}
}

// GetTemperature fetches temperature data for a specific location
func (s *TemperatureService) GetTemperature(location string) (*TemperatureResponse, error) {
	u, err := url.Parse(s.BaseURL)
	if err != nil {
		return nil, fmt.Errorf("bad base url: %w", err)
	}

	u.Path = path.Join(u.Path, "temperature")
	q := u.Query()
	q.Set("location", location)
	u.RawQuery = q.Encode()

	log.Printf("Request from %s", u.String())
	resp, err := s.HTTPClient.Get(u.String())

	if err != nil {
		return nil, fmt.Errorf("error fetching temperature data: %w", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return nil, fmt.Errorf("unexpected status code: %d", resp.StatusCode)
	}

	var temperatureResp TemperatureResponse
	if err := json.NewDecoder(resp.Body).Decode(&temperatureResp); err != nil {
		return nil, fmt.Errorf("error decoding temperature response: %w", err)
	}

	return &temperatureResp, nil
}

func (s *TemperatureService) SendToIntegrationAPI(deviceName string, data string) error {
	u, err := url.Parse(s.IntegrationURL)
	if err != nil {
		return fmt.Errorf("bad base url: %w", err)
	}
	u.Path = path.Join(u.Path, "Integration")

	payload := struct {
		DeviceName string `json:"DeviceName"`
		Data       string `json:"Data"`
	}{
		DeviceName: deviceName,
		Data:       data,
	}

	jsonData, _ := json.Marshal(payload)
	resp, err := s.HTTPClient.Post(u.String(), "application/json", bytes.NewBuffer(jsonData))

	if err != nil {
		return fmt.Errorf("failed to send request: %w", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusNoContent && resp.StatusCode != http.StatusOK {
		return fmt.Errorf("integration api returned unexpected status: %d", resp.StatusCode)
	}

	return nil
}

// GetTemperatureByID fetches temperature data for a specific sensor ID
func (s *TemperatureService) GetTemperatureByID(sensorID string) (*TemperatureResponse, error) {
	url := fmt.Sprintf("%s/temperature/%s", s.BaseURL, sensorID)

	resp, err := s.HTTPClient.Get(url)
	if err != nil {
		return nil, fmt.Errorf("error fetching temperature data: %w", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return nil, fmt.Errorf("unexpected status code: %d", resp.StatusCode)
	}

	var temperatureResp TemperatureResponse
	if err := json.NewDecoder(resp.Body).Decode(&temperatureResp); err != nil {
		return nil, fmt.Errorf("error decoding temperature response: %w", err)
	}

	return &temperatureResp, nil
}
